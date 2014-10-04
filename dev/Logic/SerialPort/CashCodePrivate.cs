using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SP = System.IO.Ports;

namespace Logic.SerialPort
{
    public class CashCode
    {
       
        public CashCodeHelper helper = new CashCodeHelper();
        
        public enum RUB : uint
        {
            B10 = 4,  //00000100
            B50 = 8,  //00001000
            B100 = 16, //00010000
            B500 = 32, //00100000
            B1000 = 64, //01000000
            B5000 = 128 //10000000
        }

        public delegate void ProcessMessage(uint error, string message);
        public delegate void PollingBill(UInt16 nominal, ref bool canLoop);

        protected SP.SerialPort Port = new SP.SerialPort();
        // Событие при любых сообщениях
        public ProcessMessage DelegateProcessMessage { get; set; }
        // Событие возникающее при приеме купюры, через переменную CanLoop можем прервать работу
        public PollingBill DelegatePollingBill { get; set; }

        // Свойство проверяющее/устанавливающее цикл опроса
        public bool CanPollingLoop { get; set; }
        // Свойство для установки номера ком порта
        public ushort NumberOfComPort { get; set; }
        // Признак установки соединения с ком портом
        public bool PortConnected { get; set; }

        protected byte[] Command = new byte[255];
        private int CommandLength = 0;
        protected byte[] Answer = new byte[255];
        private int AnswerLength = 0;
        protected byte[] Data = new byte[255];
        private int DataLength = 0;

        //Выполнение сформированной команды
        protected void ProcessComand()
        {
            var stream = this.Port.BaseStream;
            if (!stream.CanWrite) throw new Exception("Не могу записать команду");
            stream.Write(this.Command, 0, this.CommandLength);
            ClearAnswer();
            this.Answer = new byte[255];
            int i = 0, maxBytes = 255;
            for (; i < maxBytes; i++)
            {
                try
                {
                    this.Answer[i] = (byte) this.Port.ReadByte();
                    if (i == 2) 
                        maxBytes = this.Answer[2];
                }
                catch
                {
                    break;
                }
            }
            this.AnswerLength = i;
            if (this.AnswerLength == 0) throw new Exception("Не удалось прочесть ответ из порта");
            this.ParseAnswer();
        }
        // Формирование команды
        protected void SendPacket(byte command, byte[] data)
        {
            ClearCommand();
            this.Command = new Byte[255];
            this.CommandLength = 6 + data.Length;
            this.Command[0] = 0x02;
            this.Command[1] = 0x03;
            this.Command[2] = (byte)this.CommandLength;
            this.Command[3] = command;

            if (data.Length > 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    this.Command[4 + i] = data[i];
                }
            }
            var crcWord = helper.GetCRC16(this.Command, (uint)this.CommandLength - 2);
            var crc = BitConverter.GetBytes(crcWord);

            this.Command[this.CommandLength - 2] = crc[0];
            this.Command[this.CommandLength - 1] = crc[1];
        }
       
       
        

        // Основные команды валидатора
        public bool Reset() // Сброс купюро-приемника 
        {
            if (!PortConnected) throw new ArgumentException("COM порт закрыт");
            this.SendPacket(0x30, new Byte[0]);
            this.DelegateProcessMessage(204, "->RESET");
            ProcessComand();

            if (this.Data[0] == 255)
            {
                this.DelegateProcessMessage(202, "<-NSC");
                throw new Exception("Получен отрицательный ответ (NAK)");
            }
            if (this.Data[0] == 0)
            {
                this.DelegateProcessMessage(203, "<-ASC");
            }
            return true;
        }
        public bool Identification(ref string Name, ref string Namber)  //Номер и название купюроприемника
        {
            if (!PortConnected) throw new ArgumentException("COM порт закрыт");
            this.SendPacket(0x37, new Byte[0]);
            this.DelegateProcessMessage(209, "->IDENTIFICATION");
            ProcessComand();
            for (int i = 0; i < DataLength; i++)
            {
                if (i >= 0 && i < 15) Name += (char)Data[i];
                if (i >= 15 && i < 27) Namber += (char)Data[i];
            }
            return true;
        }
        public bool GetStatus(ref List<RUB> Nominal, ref List<RUB> Security)//Получение списка принимаемых купюр
        {
            if (!PortConnected) throw new ArgumentException("COM порт закрыт");
            this.SendPacket(49, new Byte[0]);
            this.DelegateProcessMessage(210, "->GET STATUS");
            ProcessComand();

            if( helper.IsBitSet(Data[2], 2)) Nominal.Add(RUB.B10);
            if (helper.IsBitSet(Data[2], 3)) Nominal.Add(RUB.B50);
            if (helper.IsBitSet(Data[2], 4)) Nominal.Add(RUB.B100);
            if (helper.IsBitSet(Data[2], 5)) Nominal.Add(RUB.B500);
            if (helper.IsBitSet(Data[2], 6)) Nominal.Add(RUB.B1000);
            if (helper.IsBitSet(Data[2], 7)) Nominal.Add(RUB.B5000);

            if (helper.IsBitSet(Data[5], 2)) Security.Add(RUB.B10);
            if (helper.IsBitSet(Data[5], 3)) Security.Add(RUB.B50);
            if (helper.IsBitSet(Data[5], 4)) Security.Add(RUB.B100);
            if (helper.IsBitSet(Data[5], 5)) Security.Add(RUB.B500);
            if (helper.IsBitSet(Data[5], 6)) Security.Add(RUB.B1000);
            if (helper.IsBitSet(Data[5], 7)) Security.Add(RUB.B5000);

            return true;
        }
        public bool EnableBillTypes(List<RUB> Nominal)        //Установка прнимаемых купюр
        {
            if (!PortConnected) throw new ArgumentException("COM порт закрыт");
            byte billTipesByte = 0;
            foreach (var item in Nominal)
            {
                billTipesByte += (byte)item;
            }

            SendPacket(52, new byte[] { 0, 0, billTipesByte, 0, 0, 0 });
            this.DelegateProcessMessage(206, "->ENABLE BILL TYPES");
            ProcessComand();

            if (Data[0] == 255)
            {
                this.DelegateProcessMessage(202, "<-NSC");
                throw new Exception("Получен отрицательный ответ (NAK)");
            }
            if (Data[0] == 0)
            {
                this.DelegateProcessMessage(203, "<-ASC");
            }
            return true;
        }
        public bool SetSecurity(List<RUB> Nominal)       //Установка повышенного контроля для определенных купюр
        {
            if (!PortConnected) throw new ArgumentException("COM порт закрыт");
            byte billTipesByte = 0;
            foreach (var item in Nominal)
            {
                billTipesByte += (byte)item;
            }

            SendPacket(50, new byte[] { 0, 0, billTipesByte});
            this.DelegateProcessMessage(207, "->SET SECURITY");
            ProcessComand();

            if (Data[0] == 255)
            {
                this.DelegateProcessMessage(202, "<-NSC");
                throw new Exception("Получен отрицательный ответ (NAK)");
            }
            if (Data[0] == 0)
            {
                this.DelegateProcessMessage(203, "<-ASC");
            }
            return true;
        }
        public bool Stack()                               //Принимаем купюру ESCROW режим
        {
            if (!PortConnected) throw new ArgumentException("COM порт закрыт");
            SendPacket(53, new byte[0]);
            this.DelegateProcessMessage(208, "->STACK");
            ProcessComand();
            return true;
        }
        public bool Return()//Возврат купюры   ESCROW режим
        {
            if (!this.PortConnected) throw new ArgumentException("Порт закрыт");
            this.SendPacket(54, new Byte[0]);
            this.DelegateProcessMessage(205, "->RETURN");
            ProcessComand();
            return true;
        }
        public bool SendASC()                                //Подтверждаем получение
        {
            if (!PortConnected) throw new ArgumentException("COM порт закрыт");
            this.SendPacket(0, new Byte[0]);
            this.DelegateProcessMessage(211, "->ASC");
            try
            {
                ProcessComand();
            }
            catch
            {
                //нормальное явление
            }
            return true;
        }
        public bool SendNSC()                                //Не подтверждааем получение
        {
            if (!PortConnected) throw new ArgumentException("COM порт закрыт");
            this.SendPacket(255, new Byte[0]);
            this.DelegateProcessMessage(212, "->NSC");
            ProcessComand();
            return true;
        }
        

        void ClearAnswer()
        {
            this.Answer = new byte[255];
            this.AnswerLength = 0;
        }
        void ClearCommand()
        {
            this.Command = new byte[255];
            this.CommandLength = 0;
        }
        void ClearData()
        {
            this.Data = new byte[255];
            this.DataLength = 0;
        }
        public void CloseComPort()
        {
            this.Port.Close();
        }
        public void OpenComPort()
        {
            uint RxBufferSize = 256;
            uint TxBufferSize = 256;

            if (this.NumberOfComPort == 0) throw new ArgumentException("Номер порта не может быть 0");
            this.Port.PortName = "COM" + this.NumberOfComPort;
            this.Port.ReadBufferSize = (int)RxBufferSize;
            this.Port.WriteBufferSize = (int)TxBufferSize;
            this.Port.ReadTimeout = 400;
            this.Port.WriteTimeout = 400;
            this.Port.Open();
            if (!this.Port.IsOpen) throw new Exception("Не удалось открыть заданный порт " + this.Port.PortName);
            

            this.Port.BaudRate = 9600;
            this.Port.Parity = System.IO.Ports.Parity.None;
            this.Port.StopBits = System.IO.Ports.StopBits.One;
            this.Port.DtrEnable = true;

            

            this.PortConnected = true;
        }

        void ParseAnswer()
        {
            this.ClearData();
            // Минимальный ответ 6 символов все что меньше не полный данные
            if (this.AnswerLength < 6) throw new ArgumentException("Не полный ответ");
            UInt16 crc16word = this.helper.GetCRC16(this.Answer, (UInt32)this.AnswerLength - 2);
            byte[] crc16 = BitConverter.GetBytes(crc16word);
            if (crc16.Length != 2) throw new Exception("что то пошло не так");
            if ((crc16[0] == this.Answer[this.AnswerLength - 2]) && (crc16[1] == this.Answer[this.AnswerLength - 1]))
            {//ответ полный и CRC верный а значит 100 % что данные верны, положим их в дату

                // 5 это 1 байт синхронизации, 1 байт адресс девайса, 1 байт - длина данных, 2 байт - CRC
                //this.Data = new byte[this.AnswerLength - 5];
                for (int i = 3, j = 0; j < this.AnswerLength - 5; i++, j++)
                {
                    this.Data[j] = this.Answer[i];
                }
                this.DataLength = this.AnswerLength - 5;
            }
            else
            {
                throw new Exception("Не полный CRC ответа");
            }
        }
        public bool Poll()
        {
            if (!this.PortConnected) throw new ArgumentException("COM порт закрыт, выполнение команды RESET не возможно");
            this.SendPacket(0x33    , new byte[0]);   //110011
            this.DelegateProcessMessage(203, "->POLL");

            this.ProcessComand();
            
            return true;
        }
        public UInt16 PollingLoop(UInt16 Sum, UInt32 TimeLoop)
        {
            var dtStart = DateTime.Now;
            this.CanPollingLoop = true;
            uint result = 0;
            while (this.CanPollingLoop)
            {
                if (this.Poll())
                {
                    #region reading
                    
                   
                    uint firstByte = this.Data[0];
                    uint secondByte = this.Data[1];

                    switch (firstByte)
                    {
                        case 16:
                        case 17:
                        case 18:
                            {
                                this.DelegateProcessMessage(213, "Включение питания после команд");
                                this.CanPollingLoop = false;
                            } break;
                        case 19:
                            {
                                this.DelegateProcessMessage(214, "Инициализация");
                            } break;
                        case 20:
                            {
                                this.DelegateProcessMessage(215, "Ожидание приема валюты");
                            } break;
                        case 21:
                            {
                                this.DelegateProcessMessage(216, "Акцепт");
                            } break;
                        case 22:
                            {
                                this.DelegateProcessMessage(217, "Недоступен, ожидание приема валюты");
                            } break;

                        case 65:
                            {
                                this.DelegateProcessMessage(218, "Полная касса");
                                this.Reset();
                                this.CanPollingLoop = false;
                            } break;
                        case 66:
                            {
                                this.DelegateProcessMessage(219, "Кассета отсутствует");
                                this.Reset();
                                this.CanPollingLoop = false;
                            } break;
                        case 67:
                            {
                                this.DelegateProcessMessage(220, "Замяло купюру");
                                this.Reset();
                                this.CanPollingLoop = false;
                            } break;
                        case 68:
                            {
                                this.DelegateProcessMessage(221, "Замяло касету 0_o");
                                this.Reset();
                                this.CanPollingLoop = false;
                            } break;
                        case 69:
                            {
                                this.DelegateProcessMessage(222, "КАРАУЛ !!!! ЖУЛИКИ !!!");
                                Reset();
                                this.CanPollingLoop = false;
                            } break;
                        case 70:
                            {
                                this.DelegateProcessMessage(223, "Сбой оборудования");
                                switch (secondByte)
                                {
                                    case 80: this.DelegateProcessMessage(224, "Stack_motor_falure"); break;
                                    case 81: this.DelegateProcessMessage(225, "Transport_speed_motor_falure"); break;
                                    case 82: this.DelegateProcessMessage(226, "Transport-motor_falure"); break;
                                    case 83: this.DelegateProcessMessage(227, "Aligning_motor_falure"); break;
                                    case 84: this.DelegateProcessMessage(228, "Initial_cassete_falure"); break;
                                    case 85: this.DelegateProcessMessage(229, "Optical_canal_falure"); break;
                                    case 86: this.DelegateProcessMessage(230, "Magnetical_canal_falure"); break;
                                    case 95: this.DelegateProcessMessage(231, "Capacitance_canal_falure"); break;
                                }
                            } break;
                        case 28:
                            {
                                this.DelegateProcessMessage(232, "Отказ от приема");
                                switch (secondByte)
                                {
                                    case 96: this.DelegateProcessMessage(233, "Insertion_error"); break;
                                    case 97: this.DelegateProcessMessage(234, "Dielectric_error"); break;
                                    case 98: this.DelegateProcessMessage(235, "Previously_inserted_bill_remains_in_head"); break;
                                    case 99: this.DelegateProcessMessage(236, "Compensation__factor_error"); break;
                                    case 100: this.DelegateProcessMessage(237, "Bill_transport_error"); break;
                                    case 101: this.DelegateProcessMessage(238, "Identification_error"); break;
                                    case 102: this.DelegateProcessMessage(239, "Verification_error"); break;
                                    case 103: this.DelegateProcessMessage(240, "Optic_sensor_error"); break;
                                    case 104: this.DelegateProcessMessage(241, "Return_by_inhibit_error"); break;
                                    case 105: this.DelegateProcessMessage(242, "Capacistance_error"); break;
                                    case 106: this.DelegateProcessMessage(243, "Operation_error"); break;
                                    case 107: this.DelegateProcessMessage(244, "Length_error"); break;
                                }
                            } break;
                        case 128:
                            {
                                this.DelegateProcessMessage(245, "Делонет");
                            } break;
                        case 129:
                            {

                                this.DelegateProcessMessage(246, "Укладка");
                                UInt16 BillNominal = 0;
                                switch (secondByte)
                                {
                                    case 2: BillNominal = 10; break;
                                    case 3: BillNominal = 50; break;
                                    case 4: BillNominal = 100; break;
                                    case 5: BillNominal = 500; break;
                                    case 6: BillNominal = 1000; break;
                                    case 7: BillNominal = 5000; break;
                                }

                                dtStart = DateTime.Now; // Сбросим время до ожидания следующей купюры
                                SendASC();        // Подтвердим прием
                                result += BillNominal;
                                bool refCanPL = this.CanPollingLoop;
                                this.DelegatePollingBill(BillNominal, ref refCanPL);  // Дадим сообщение о принятой купюре
                                this.CanPollingLoop = refCanPL;
                                if (result >= Sum)
                                {

                                    this.CanPollingLoop = false;  //Сумма набралась, заканчиваем клянчить
                                    this.DelegateProcessMessage(247, "Набрали нужную сумму");
                                }


                            } break;
                        case 130: { this.DelegateProcessMessage(248, "Возврат купюры"); } break;

                    }
                    Thread.Sleep(200);
                    #endregion
                }
                if ((DateTime.Now - dtStart).Seconds > TimeLoop)
                {
                    this.CanPollingLoop = false;
                    this.DelegateProcessMessage(249, "Завершаем работу по таймауту");
                }
            }
           
            return (ushort)result;
        }

    }

    public class CashCodeHelper
    {
        const int POLYNOM = 33800;  //0x08408, 1000010000001000
        internal UInt16 GetCRC16(byte[] InData, UInt32 DataLng)
        {
            int res = 0;
            int crc = 0;
            for (int i = 0; i < DataLng; i++)
            {
                crc = res ^  InData[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc = crc >> 1;
                        crc = crc ^ POLYNOM;
                    }
                    else
                    {
                        crc = crc >> 1;
                    }
                }
                res = crc;
            }
            return (UInt16) res;
        }

        // Проверка заданного бита на 1
        internal bool IsBitSet(byte Value, int BitNum)
        {
            return ((Value >> BitNum) & 1) == 1;
        }

        // Установка заданного бита в 1
        internal byte BitOn(byte val, byte theBit)
        {
            return (byte)(val | (1 <<  theBit) );
        }

        // Установка заданного бита в 0
        internal byte BitOff(byte val, byte TheBit)
        {
            return (byte) (val & ((1 << TheBit) ^ 0xFF));
        }


    }
}
