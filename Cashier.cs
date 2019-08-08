using System;
using System.Threading;

namespace SupermarketSimulator
{
    public class Cashier
    {
        public string CashierNumber { get; set; }

        public Cashier(int cashierNumber)
        {
            this.CashierNumber = cashierNumber.ToString();
        }
        
        public void StartProcessing(int ProcessingTime) {
            Console.WriteLine("Cashier {0} start process, it will take him {1} to process all prodacts.", this.CashierNumber, ProcessingTime.ToString());
            Thread.Sleep(ProcessingTime * 1000);
            Console.WriteLine("Cashier {0} finished to process all prodacts.", this.CashierNumber);
        }
    }
}
