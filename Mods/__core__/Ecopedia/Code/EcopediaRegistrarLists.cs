// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Core.Systems;
    using Eco.Gameplay.Civics.Misc;
    using Eco.Gameplay.EcopediaRoot;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Utils;
    using Eco.Shared.Items;
    using System.Collections.Generic;
    using System.Linq;
    using Eco.Gameplay.Items;
    using Eco.Shared.Localization;

    //This class creates pages in the Ecopedia for each thing stored in a Registrar.
    //You can add new types of generated data to the Eco tree in mods by deriving a new mod class from IEcopediaGeneratedData and implementing it.
    public class EcopediaRegistrarLists : IEcopediaGeneratedData
    {
        public IEnumerable<EcopediaPageReference> PagesWeSupplyDataFor() => Registrars.AllRegistrars.Where(r => r.ShowInEcopedia).Select(x=>new EcopediaPageReference(x.RegistrarName.NotTranslated, "World Index", x.EcopediaPageName, x.RegistrarName)).DistinctBy(x=>x.Page);

        public LocString GetEcopediaData(Player player, EcopediaPage page)
        {
            var regs = Registrars.AllRegistrars.Where(x=>x.EcopediaPageName == page.Name);
            var sb = new LocStringBuilder();
            foreach(var reg in regs)
                sb.AppendLine(reg.All().
                    Where(x => (x as IEcopediaEntry)?.IsVisibleInEcopedia ?? true)
                    .OrderBy(x=> this.SortOrder(x as IProposable))
                    .ThenBy(x=>x.Name)
                    .Select(x=>new LocString(x.MarkedUpName))
                    .MakeList(reg.RegistrarName));
            return sb.ToLocString();
        }

        //When sorting proposables, put active at the top. 
        int SortOrder(IProposable p)
        {
            if (p == null) return 0;
            if (p.State == ProposableState.Active) return -1;
            return (int)p.State;
        }
    }
}
