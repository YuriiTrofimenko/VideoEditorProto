using System.Data.Entity;
using System.Linq;

namespace VideoEditorProto.Domain.Abstract
{
    public interface IProjectRepository
    {
        IQueryable<Project> Project { get; }
        IQueryable<AudioCodecs> AudioCodecs { get; }
        IQueryable<VideoCodecs> VideoCodecs { get; }
    }
}
