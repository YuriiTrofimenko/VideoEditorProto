using System;
using System.Data.Entity;
using System.Linq;
using VideoEditorProto.Domain.Abstract;

namespace VideoEditorProto.Domain.Concrete
{
    public class EFProjectRepostory : IProjectRepository
    {
        public edoEntities context = new edoEntities();

        IQueryable<Project> IProjectRepository.Project { get => context.Projects.Include("AudioCodec").Include("VideoCodec"); }
        IQueryable<AudioCodec> IProjectRepository.AudioCodecs { get => context.AudioCodecs; }
        IQueryable<VideoCodec> IProjectRepository.VideoCodecs { get => context.VideoCodecs; }
    }
}
