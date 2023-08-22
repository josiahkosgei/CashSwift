// Decompiled with JetBrains decompiler
// Type: LastDroppedNotes
// Assembly: CashAccSysDeviceManager, Version=6.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 3301DEC8-4781-402A-85F0-62E202788F00
// Assembly location: C:\DEV\maniwa\bak\New folder\6.0 - Demo\CashAccSysDeviceManager.dll

using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "LastDroppedNotes")]
    public class LastDroppedNotes
    {
        [XmlElement(ElementName = "NoteCount")]
        public List<NoteCount> NoteCount { get; set; }
    }
}
