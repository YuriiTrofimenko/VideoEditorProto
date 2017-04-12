using System.Data.Entity;
using System.Linq;

namespace VideoEditorProto.Domain.Abstract
{
    /*Интерфейс репозитория данных проектов*/
    public interface IProjectRepository
    {
        IQueryable<Project> Project { get; }
        IQueryable<Project> EagerProject { get; }
        bool SaveProject(Project project);

        IQueryable<AudioCodec> AudioCodecs { get; }

        IQueryable<VideoCodec> VideoCodecs { get; }
    }
}
