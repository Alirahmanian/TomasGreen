using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Packing.ViewModels
{
    public class PackingPlanViewModel
    {
        public PackingPlan PackingPlan { get; set; }
        public PackingPlanMix PackingPlanMix { get; set; }
        public PackingPlanMixArticle PackingPlanMixArticle { get; set; }
    }
}
