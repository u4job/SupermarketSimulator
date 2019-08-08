using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SupermarketSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfCashiers = 0;
            int maxProcessingTime = 0;

            //safely enter number of cashiers
            do
            {
                Console.Write("Enter number of cashiers: ");
                string strNumberOfCashiers = Console.ReadLine();

                try
                {
                    numberOfCashiers = int.Parse(strNumberOfCashiers);
                }
                catch
                { }
            } while (numberOfCashiers <= 0);

            //new cashier list
            List<Cashier> cashierList = new List<Cashier>();

            //add new cashiers into the list
            for (int i = 0; i < numberOfCashiers; i++)
            {
                cashierList.Add(new Cashier(i + 1));
            }

            //safely enter maximum processing time
            do
            {
                Console.Write("Enter maximum processing time(min 3): ");
                string strMaxProcessingTime = Console.ReadLine();

                try
                {
                    maxProcessingTime = int.Parse(strMaxProcessingTime);
                }
                catch
                { }
            } while (maxProcessingTime <= 3);

            //new customers queue - every customer as random processing time
            Queue<int> customers = new Queue<int>();

            //to calculat the processing time of the customer
            Random processTime = new Random();

            //to close the Supermarket - no new customer will be add
            bool SupermarketOpen = true;

            //new thread that will add new customers every second
            Thread AddCustomers = new Thread(delegate ()
            {
                while (SupermarketOpen)
                {
                    customers.Enqueue(processTime.Next(1, maxProcessingTime));
                    Console.WriteLine("There is " + customers.Count + " customers is the queue.");
                    Thread.Sleep(1000);
                }
            });

            //new thread that will close the supermarket on any key
            Thread closeSupermarket = new Thread(delegate ()
            {
                Console.Read();
                SupermarketOpen = false;
            });

            //start the threads to add customers and close supermarket
            AddCustomers.Start();
            closeSupermarket.Start();

            //start multiple threads for each cashiers
            Parallel.For(0, cashierList.Count, (i) =>
            {
                //if supermarket open or there is still customers in the queue
                while (SupermarketOpen || (SupermarketOpen == false && customers.Count != 0))
                {
                    //get the next customer from the queue if there is any
                    int tempProcessTime = -1;
                    try
                    {
                        tempProcessTime = customers.Dequeue();
                    }
                    catch { }

                    if (tempProcessTime != -1)
                    {
                        cashierList[i].StartProcessing(tempProcessTime);
                    }
                }
            });
        }
    }
}
