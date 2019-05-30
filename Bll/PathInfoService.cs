using IBll;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll
{
    public partial class PathInfoService : BaseService<PathInfo>, IPathInfoService
    {
        public bool SetStatus(int id, int status)
        {
            PathInfo path = CurrentDal.GetEntities(u => u.ID == id).FirstOrDefault();
            path.Status = status;
            if (Update(path))
            {
                return true;
            }
            return false;
        }
    }
}
