using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.CheckLists
{
    [Table("CheckLists")]
    public class CheckListEntity
    {
        [Column("CheckListId")]
        public int CheckListEntityId { get; set; }
        public string CheckListName { get; set; }
        public List<CheckListItem> CheckListItems { get; set; }
        public virtual List<Component> Components { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual User CreatorCheckListUser { get; set; }
        public virtual User LastEditorCheckListUser { get; set; }
        public virtual DateTime LastEditionDateTime { get; set; }
        public CheckListEntity()
        {
            this.Components = new List<Component>();
        }
    }
}
