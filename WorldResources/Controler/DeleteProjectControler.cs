using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldResources.Controler
{
    class DeleteProjectControler
    {
        public DeleteProjectControler()
        {
            GlowingEarth.getInstance().getMaster().getResources().Clear();
            GlowingEarth.getInstance().getMaster().getTypes().Clear();
            GlowingEarth.getInstance().getMaster().getTags().Clear();
            GlowingEarth.getInstance().getMaster().getMapItems().Clear();
            GlowingEarth.getInstance().getMaster().setTitle(GlowingEarth.getInstance().getMaster().getTitle().Replace("*",string.Empty));
        }
    }
}
