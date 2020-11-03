using System;
using System.Collections.Generic;
using System.Text;

namespace Parranze.Data
{
    public interface IDataEntity
    {
        public Guid ID { get; set; }
    }
}
