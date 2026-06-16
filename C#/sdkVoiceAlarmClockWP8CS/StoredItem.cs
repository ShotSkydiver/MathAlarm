using System.IO.IsolatedStorage;

namespace MathClock
{
    // Encapsulates a key/value pair stored in isolated storage. 
    public class StoredItem<T>
    {
        private T value;
        private T defaultValue;
        private string name;
        private bool needRefresh;

        public StoredItem(string _name, T _defaultValue)
        {
            this.name = _name;
            this.defaultValue = _defaultValue;

            // If isolated storage doesn't have the value stored yet
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(this.name, out this.value))
            {
                this.value = _defaultValue;
                IsolatedStorageSettings.ApplicationSettings[this.name] = _defaultValue;
            }

            this.needRefresh = false;
        }

        public T Value
        {
            get
            {
                if (this.needRefresh)
                {
                    if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(this.name, out this.value))
                    {
                        IsolatedStorageSettings.ApplicationSettings[this.name] = this.defaultValue;
                        this.value = this.defaultValue;
                    }
                    this.needRefresh = false;
                }

                return this.value;
            }
            set
            {
                if (this.value.Equals(value))
                    return;

                // Store the value in isolated storage.
                IsolatedStorageSettings.ApplicationSettings[this.name] = value;
                this.needRefresh = true;
            }
        }

        public T DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public override string ToString()
        {
            return this.name
                + " with value: " + this.value.ToString()
                + ", default value: " + this.defaultValue.ToString();
        }

        public void ForceRefresh()
        {
            this.needRefresh = true;
        }
    }
}
