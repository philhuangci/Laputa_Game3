using System;
using System.Collections;
using System.Collections.Generic;

namespace Model
{
    public enum ObjectType
    {
        None
    }

    [Serializable]
    public class Identity
    {
        public string ID;
        public string Name;
        public string Description;
        public ObjectType Type;
        public bool IsShow;

        public Identity() { Type = ObjectType.None; }  

        public Identity(string id, string name, string description, ObjectType type)
        {
            ID = id;
            Name = name;
            Description = description;
            Type = type;
            IsShow = true;
        }

        public void SetIdentity(Identity identity)
        {
            ID = identity.ID;
            Name = identity.Name;
            Description = identity.Description;
            Type = identity.Type;
            IsShow = identity.IsShow;
        }

        public static ObjectType ToObjectType(string type)
        {
            switch (type)
            {
                
            }
            return ObjectType.None;
        }
    }

    [Serializable]
    public class IdentityList
    {
        public List<Identity> Identities;

        public IdentityList()
        {
            Identities = new List<Identity>();
        }

        public bool NameExists(string name)
        {
            foreach(var id in Identities)
            {
                if(id.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool NameExists(Identity identity)
        {
            foreach (var id in Identities)
            {
                if (identity.ID != id.ID && identity.Name == id.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IDExists(string id)
        {
            foreach (var identity in Identities)
            {
                if (identity.ID == id)
                {
                    return true;
                }
            }

            return false;
        }
        
        public Identity GetIdentity(string id)
        {
            foreach(var identity in Identities)
            { 
                if (identity.ID == id)
                {
                    return identity;
                }
            }

            return null;
        }

        public int Count
        {
            get
            {
                return Identities.Count;
            }
        }

        public void Add(Identity identity)
        {
            foreach (var item in Identities)
            {
                if (item.ID == identity.ID)
                {
                    return;
                }
            }

            Identities.Add(identity);
        }

        public void AddRange(List<Identity> identities)
        {
            Identities.AddRange(identities);
        }

        public void RemoveByID(string id)
        {
            foreach (var identity in Identities)
            {
                if (identity.ID == id)
                {
                    Identities.Remove(identity);
                    return;
                }
            }
        }
        public void ClearIdentities()
        {
            Identities.Clear();
        }
    }

}
