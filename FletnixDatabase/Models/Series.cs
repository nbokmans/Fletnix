//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FletnixDatabase.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Series
    {
        public Series()
        {
            this.Season = new HashSet<Season>();
            this.Genre = new HashSet<Genre>();
        }
    
        public int SeriesID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartYear { get; set; }
        public Nullable<System.DateTime> EndYear { get; set; }
    
        public virtual ICollection<Season> Season { get; set; }
        public virtual ICollection<Genre> Genre { get; set; }
    }
}
