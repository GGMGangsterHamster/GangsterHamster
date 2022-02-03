using System;
using System.Collections.Generic;

namespace Objects.UI.VO
{
    [Serializable]
    public class DialogVO
    {
        public List<DialogDataVO> dialogs;

        public DialogVO(List<DialogDataVO> dialogs)
        {
            this.dialogs = dialogs;
        }
    }

    [Serializable]
    public class DialogDataVO
    {
        public int id;
        public List<InnerDialogVO> dialog;

        public DialogDataVO(int id, List<InnerDialogVO> dialog)
        {
            this.id = id;
            this.dialog = dialog;
        }
    }

    [Serializable]
    public class InnerDialogVO
    {
        public int index;
        public string message;
        public int iconID;
        public int customCallbackID;

        public InnerDialogVO(int index, string message, int iconID, int customCallbackID)
        {
            this.index = index;
            this.message = message;
            this.iconID = iconID;
            this.customCallbackID = customCallbackID;
        }
    }
}