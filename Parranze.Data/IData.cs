using System;
using System.Collections.Generic;
using System.Text;

namespace Parranze.Data
{
    public interface IData
    {
        public IEnumerable<IDataEntity> GetAll();
    }
}
