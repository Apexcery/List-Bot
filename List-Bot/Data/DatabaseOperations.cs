using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using List_Bot.Models;

namespace List_Bot.Data
{
    public interface IDatabaseOperations
    {
        Task<List> FindByName(string name);
        Task<bool> AddNewList(List list);
        Task<bool> DeleteListByName(string name);
        Task<List<List>> GetAllLists();

        Task<bool> AddToList(List list, ListItem item);
        Task<bool> RemoveFromList(List list, ListItem item);
    }

    public class DatabaseOperations : IDatabaseOperations
    {
        private readonly DatabaseContext _context;

        public DatabaseOperations(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List> FindByName(string name)
        {
            var list = await _context.Lists.FirstOrDefaultAsync(x =>
                x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            return list;
        }

        public async Task<bool> AddNewList(List list)
        {
            await _context.Lists.AddAsync(list);
            var entitiesUpdated = await _context.SaveChangesAsync();
            
            return entitiesUpdated > 0;
        }

        public async Task<bool> DeleteListByName(string name)
        {
            var list = await FindByName(name);
            if (list == null)
                return false;

            _context.Lists.Remove(list);
            var entitiesUpdated = await _context.SaveChangesAsync();

            return entitiesUpdated > 0;
        }

        public async Task<List<List>> GetAllLists()
        {
            var lists = await _context.Lists.ToListAsync();
            return lists;
        }

        public async Task<bool> AddToList(List list, ListItem item)
        {
            var updatedList = list;
            updatedList.Items.Add(item);
            updatedList.UpdatedDate = DateTime.UtcNow;

            _context.Lists.Update(updatedList);
            var entitiesUpdated = await _context.SaveChangesAsync();

            return entitiesUpdated > 0;
        }

        public async Task<bool> RemoveFromList(List list, ListItem item)
        {
            var updatedList = list;
            if (!list.Items.Contains(item))
                return false;

            updatedList.Items.Remove(item);
            updatedList.UpdatedDate = DateTime.UtcNow;

            _context.Lists.Update(updatedList);
            var entitiesUpdated = await _context.SaveChangesAsync();

            return entitiesUpdated > 0;
        }
    }
}
