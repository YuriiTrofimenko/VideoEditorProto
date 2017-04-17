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

        public bool SaveProject(Project _project)
        {
            bool result = true;
            try
            {
                Project dbEntry = context.Projects.Find(_project.Id);
                //Если запись о проекте существует - обновляем ее данные
                if (dbEntry != null)
                {
                    dbEntry.Width = _project.Width;
                    dbEntry.Height = _project.Height;
                }
                //Если нет - создаем запись
                else
                {
                    context.Projects.Add(_project);
                }
                context.SaveChanges();
            }
            catch (Exception)
            {

                result = false;
            }
            return result;
        }

        IQueryable<AudioCodec> IProjectRepository.AudioCodecs { get => context.AudioCodecs; }

        IQueryable<VideoCodec> IProjectRepository.VideoCodecs { get => context.VideoCodecs; }
    }
}
