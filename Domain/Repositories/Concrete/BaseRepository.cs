using Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Domain.Repositories.Concrete
{
    public class BaseRepository<Entity> : IBaseRepository<Entity> where Entity : class
    {
        private readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Insert(Entity entity)
        {
            _context.Set<Entity>().Add(entity);
        }

        public void Modify(Entity entity, int id)
        {
            Entity oldEntity = _context.Set<Entity>().Find(id);
            oldEntity = entity;
            _context.Set<Entity>().Update(oldEntity);
        }

        public IQueryable<Entity> ObtainAll()
        {
            return _context.Set<Entity>().AsQueryable();
        }

        public Entity ObtainById(int id)
        {
            return _context.Set<Entity>().Find(id);
        }

        public void Remove(int id)
        {
            Entity entity = ObtainById(id);
            _context.Set<Entity>().Remove(entity);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
