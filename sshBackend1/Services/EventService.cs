using sshBackend1.Data;
using sshBackend1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sshBackend1.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<Event> CreateEventAsync(Event newEvent)
        {
            if (string.IsNullOrEmpty(newEvent.EventName))
            {
                throw new ArgumentException("EventName nuk mund të jetë bosh.");
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<Event?> UpdateEventAsync(int id, Event updatedEvent)
        {
            var eventToUpdate = await _context.Events.FindAsync(id);
            if (eventToUpdate == null) return null;

            // Përditëso fushat ekzistuese
            eventToUpdate.EventName = updatedEvent.EventName;

            // Kontrollo nëse ekziston një fushë date në modelin Event
            if (updatedEvent is { EventDate: not null })
            {
                eventToUpdate.EventDate = updatedEvent.EventDate;
            }

            await _context.SaveChangesAsync();
            return eventToUpdate;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete == null) return false;

            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
