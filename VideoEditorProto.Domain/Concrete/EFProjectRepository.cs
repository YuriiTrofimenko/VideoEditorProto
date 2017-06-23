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
            catch (Exception ex)
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
            catch (Exception ex)
            {

            }
            return result;
        }

        IQueryable<Layer> Layers { get => context.Layers; }
        public Layer SaveLayer(Layer _layer)
        {
            Layer result = null;
            try
            {
                Layer dbEntry = context.Layers.Find(_layer.Id);
                //Если запись существует - обновляем ее данные
                if (dbEntry != null)
                {
                    dbEntry.Muted_Showed = _layer.Muted_Showed;
                    dbEntry.Blocked = _layer.Blocked;
                }
                //Если нет - создаем запись
                else
                {
                    context.Layers.Add(_layer);
                }
                context.SaveChanges();
                result = context.Layers.Find(_layer.Id);
            }
            catch (Exception)
            {

            }
            return result;
        }


        IQueryable<Row> Rows { get => context.Rows; }
        public Row SaveRows(Row _row)
        {
            Row result = null;
            try
            {
                Row dbRow = context.Rows.Find(_row.Id);
                //Если запись о проекте существует - обновляем ее данные
                if (dbRow != null)
                {
                    dbRow.Layer = _row.Layer;
                    dbRow.MaterialFile = _row.MaterialFile;
                    dbRow.VersionNum = _row.VersionNum;
                }
                //Если нет - создаем запись
                else
                {
                    context.Rows.Add(_row);
                }
                context.SaveChanges();
                result = context.Rows.Find(_row.Id);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        IQueryable<Effect> Effects { get => context.Effects; }
        public Effect SaveEffect(Effect _effect)
        {
            Effect result = null;
            try
            {
                Effect dbEffect = context.Effects.Find(_effect.Id);
                //Если запись о проекте существует - обновляем ее данные
                if (dbEffect != null)
                {
                    dbEffect.Version = _effect.Version;
                    dbEffect.VersionNum = _effect.VersionNum;
                }
                //Если нет - создаем запись
                else
                {
                    context.Effects.Add(_effect);
                }
                context.SaveChanges();
                result = context.Effects.Find(_effect.Id);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        IQueryable<Layer> IProjectRepository.Layers { get => context.Layers; }

        IQueryable<AudioCodec> IProjectRepository.AudioCodecs { get => context.AudioCodecs; }

        IQueryable<VideoCodec> IProjectRepository.VideoCodecs { get => context.VideoCodecs; }

        IQueryable<Row> IProjectRepository.Rows => throw new NotImplementedException();

        IQueryable<Effect> IProjectRepository.Effects => throw new NotImplementedException();
    }
}
