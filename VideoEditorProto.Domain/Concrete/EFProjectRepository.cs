using System;
using System.Data.Entity;
using System.Linq;
using VideoEditorProto.Domain.Abstract;

namespace VideoEditorProto.Domain.Concrete
{
    /*Реализация репозитория данных проектов.
     Является прослойкой между кодом контроллеров и EF6*/
    public class EFProjectRepository : IProjectRepository
    {
        public Entities context = new Entities();

        IQueryable<Project> IProjectRepository.Project { get => context.Projects; }
        //Вариант с явным указанием заранее включаемых в результат полей данных
        IQueryable<Project> IProjectRepository.EagerProject
        {
            get => context.Projects
                .Include("User")
                /*.Include("AudioCodec")
                .Include("VideoCodec")*/;
        }

        IQueryable<User> IProjectRepository.User { get => context.Users; }

        public User SaveUser(User _user)
        {
            User result = null;
            try
            {
                User dbEntry = context.Users.Find(_user.Id);
                //Если запись о проекте существует - обновляем ее данные
                if (dbEntry != null)
                {
                    dbEntry.Name = _user.Name;
                    dbEntry.Surname = _user.Surname;
                    dbEntry.Email = _user.Email;
                    dbEntry.Password = _user.Password;
                }
                //Если нет - создаем запись
                else
                {
                    context.Users.Add(_user);
                }
                context.SaveChanges();
                result = context.Users.Find(_user.Id);
            }
            catch (Exception)
            {

            }
            return result;
        }

        public Project SaveProject(Project _project)
        {
            Project result = null;
            try
            {
                Project dbEntry = context.Projects.Find(_project.Id);
                //Если запись о проекте существует - обновляем ее данные
                if (dbEntry != null)
                {
                    dbEntry.Name = _project.Name;
                }
                //Если нет - создаем запись
                else
                {
                    context.Projects.Add(_project);
                }
                context.SaveChanges();
                result = context.Projects.Find(_project.Id);
            }
            catch (Exception)
            {

            }
            return result;
        }

        IQueryable<AudioCodec> IProjectRepository.AudioCodecs { get => context.AudioCodecs; }

        IQueryable<VideoCodec> IProjectRepository.VideoCodecs { get => context.VideoCodecs; }
    }
}
