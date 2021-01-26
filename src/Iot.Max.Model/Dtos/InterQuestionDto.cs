using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Dtos
{
    public class InterQuestionDto
    {

        public string ID { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }
        public string CategoryName { get; set; }
        public int ReadCount { get; set; }
        public string Images { get; set; }
    }
}
