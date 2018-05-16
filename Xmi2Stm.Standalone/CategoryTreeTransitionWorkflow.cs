using System;
using System.Threading.Tasks;

namespace Pixstock.Applus.Foundations.ContentBrowser.Transitions
{
    public partial class CategoryTreeTransitionWorkflow
    {
        public async Task OnCategoryTreeTop_Entry()
        {
            Console.WriteLine("Call OnCategoryTreeTop_Entry");
        }

        public async Task OnCategoryTreeTop_Exit()
        {
            Console.WriteLine("Call OnCategoryTreeTop_Exit");
        }

        public async Task OnCategoryTreeIdle_Entry()
        {
            Console.WriteLine("Call OnCategoryTreeIdle_Entry");
        }

        public async Task OnCategoryTreeIdle_Exit()
        {
            Console.WriteLine("Call OnCategoryTreeIdle_Exit");
        }

        public async Task OnCategoryTreeLoading_Entry()
        {
            Console.WriteLine("Call OnCategoryTreeLoading_Entry");
        }

        public async Task OnCategoryTreeLoading_Exit()
        {
            Console.WriteLine("Call OnCategoryTreeLoading_Exit");
        }
    }
}