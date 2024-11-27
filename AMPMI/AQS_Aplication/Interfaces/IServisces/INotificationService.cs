using Domin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQS_Aplication.Interfaces.IServisces
{
    public interface INotificationService
    {
        Task<bool> Create(Notification notification);
        Task<Notification> ReadById(int id);
        Task<List<Notification>> Read();
        Task<Notification> Update(Notification notification);
        Task Delete(int id);
    }

}
