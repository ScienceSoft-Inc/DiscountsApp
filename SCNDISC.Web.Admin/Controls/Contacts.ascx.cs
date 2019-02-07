using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using SCNDISC.Web.Admin.ServiceLayer;

namespace SCNDISC.Web.Admin.Controls
{
    public partial class Contacts : UserControl
    {
        private readonly List<Contact> _contacts = new List<Contact>();

        public List<TipForm> Value
        {
            set
            {
                _contacts.ForEach(x => x.Value = null);
                for (var i = 0; i < _contacts.Count; i++)
                {
                    InitContact(_contacts[i], i, value);
                }
                RevealRemoveOptionIfPossible();
            }
            get { return _contacts.Where(x => x.Visible).Select(x => x.Value).ToList(); }
        }

        private void RevealRemoveOptionIfPossible()
        {
            uxRemove.Visible = _contacts.Count(x => x.Visible) > 1;
        }


        public void SwitchLanguage(Language language)
        {
            uxContact1.SwitchLanguage(language);
            uxContact2.SwitchLanguage(language);
            uxContact3.SwitchLanguage(language);
            uxContact4.SwitchLanguage(language);
            uxContact5.SwitchLanguage(language);
            uxContact6.SwitchLanguage(language);
            uxContact7.SwitchLanguage(language);
            uxContact8.SwitchLanguage(language);
            uxContact9.SwitchLanguage(language);
            uxContact10.SwitchLanguage(language);
        }

        protected override void OnInit(EventArgs e)
        {
            _contacts.Add(uxContact1);
            _contacts.Add(uxContact2);
            _contacts.Add(uxContact3);
            _contacts.Add(uxContact4);
            _contacts.Add(uxContact5);
            _contacts.Add(uxContact6);
            _contacts.Add(uxContact7);
            _contacts.Add(uxContact8);
            _contacts.Add(uxContact9);
            _contacts.Add(uxContact10);
            base.OnInit(e);
        }

        private void InitContact(Contact contact, int index, List<TipForm> tips)
        {
            if (index > (tips.Count - 1))
                return;

            contact.Value = tips[index];
        }

        protected void OnAddContact(object sender, EventArgs e)
        {
            var hiddenContact = _contacts.FirstOrDefault(c => !c.Visible);
            hiddenContact.Visible = true;
            hiddenContact.Clear();
            RevealRemoveOptionIfPossible();
            HideAddOptionIfPossible();
        }

        protected void OnDeleteContact(object sender, EventArgs e)
        {
            _contacts.Reverse();
            var invisible = _contacts.FirstOrDefault(c => c.Visible);
            invisible.Visible = false;
            RevealRemoveOptionIfPossible();
            HideAddOptionIfPossible();
        }

        private void HideAddOptionIfPossible()
        {
            uxAdd.Visible = _contacts.Count(x => x.Visible) != _contacts.Count;
        }
    }
}