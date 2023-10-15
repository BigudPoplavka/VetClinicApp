using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinicApp
{
    public interface ISerializable
    {
        public string Path { get; }
        public void Serialize();
    }
}
