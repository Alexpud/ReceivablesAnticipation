using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Repositories.Abstract
{
    public interface IBaseRepository <Entity>
    {
        void Insert(Entity entity);
        void Modify(Entity entity, int id);
        void Remove(int id);
        Entity ObtainById(int id);
        IQueryable<Entity> ObtainAll();
    }
}
