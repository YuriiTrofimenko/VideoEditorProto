using System.Data.Entity;
using System.Linq;

namespace VideoEditorProto.Domain.Abstract
{
    /*Интерфейс репозитория данных проектов*/
    public interface IProjectRepository
    {
        IQueryable<Project> Project { get; }
        IQueryable<Project> EagerProject { get; }
        Project SaveProject(Project _project);

        IQueryable<User> User { get; }
        User SaveUser(User _user);

        IQueryable<AudioCodec> AudioCodecs { get; }

        IQueryable<VideoCodec> VideoCodecs { get; }
    }
}
