using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharedlayer.Dto
{
    public class ErrorDto
    {
        public List<string> Errors { get; private set; } = new List<string>();
        public bool IsShow { get;private set; }


        public ErrorDto(string error,bool isshow)
        {
            Errors.Add(error);
            isshow = true;
        }
        public ErrorDto(List<string> errors,bool isshow)
        {
            Errors = errors;
            IsShow = isshow;
        }
    }
}
