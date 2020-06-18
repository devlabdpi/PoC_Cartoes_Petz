using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleCloudVision
{
    public class Response
    {
        public virtual long TotemID { get; set; }
        public virtual int NumeroImagem { get; set; }
        public virtual bool Sucesso { get; set; }
        public virtual string Descricao { get; set; }
        public virtual bool ConteudoSeguro { get; set; }
        public virtual bool ContemAnimal { get; set; }
        public virtual bool ContemLogomarca { get; set; }
        public virtual bool ContemPessoa { get; set; }
        public virtual string MediaLink { get; set; }
    }
}
