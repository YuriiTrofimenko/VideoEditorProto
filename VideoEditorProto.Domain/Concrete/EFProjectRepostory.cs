using System;
using System.Data.Entity;
using System.Linq;
using VideoEditorProto.Domain.Abstract;

namespace VideoEditorProto.Domain.Concrete
{
    public class EFProjectRepostory : IProjectRepository
    {
        public edoEntities context = new edoEntities();

        IQueryable<Project> IProjectRepository.Project { get => context.Project; }
        IQueryable<AudioCodecs> IProjectRepository.AudioCodecs { get => context.AudioCodecs; }
        IQueryable<VideoCodecs> IProjectRepository.VideoCodecs { get => context.VideoCodecs; }
    }
}
