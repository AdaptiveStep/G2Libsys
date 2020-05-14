using System;

namespace G2Libsys.Library
{
    public class Seminar
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PlannedDate { get; set; }
        public Author Author { get; set; }

        //Todo, maybe make it into a URI object?
        public string ImageSRC { get; set; }

    }
}
