using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAMvc.Entities
{
    public class MovieDirector
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        public int DirectorId { get; set; }
        public virtual Director Director { get; set; }
    }
}