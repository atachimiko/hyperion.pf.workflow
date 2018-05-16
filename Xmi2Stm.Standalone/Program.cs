using System;
using System.Threading;
using System.Threading.Tasks;
using Pixstock.Applus.Foundations.ContentBrowser.Transitions;
using Pixstock.Core;

namespace Xmi2Stm.Standalone
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Application");

            var statemachine = new CategoryTreeTransitionWorkflow();
            statemachine.Setup();
            statemachine.Start();

            Console.WriteLine("End StartUp");

            
            Console.WriteLine("Fire TRNS_TOPSCREEN");
            statemachine.Fire(Events.TRNS_TOPSCREEN, null);
            
            Thread.Sleep(1000 * 1);

            Console.WriteLine("Fire TRNS_BACK");
            statemachine.Fire(Events.TRNS_BACK, null);

            Thread.Sleep(1000 * 1);

            Console.WriteLine("End Application");
        }
    }
}
