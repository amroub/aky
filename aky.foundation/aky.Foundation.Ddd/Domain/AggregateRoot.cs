namespace aky.Foundation.Ddd.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using aky.Foundation.Ddd.Infrastructure;

    public abstract class AggregateRoot
    {
        private readonly List<DomainEvent> changes = new List<DomainEvent>();

        private int currentVersion;

        public IEnumerable<DomainEvent> GetUncommittedChanges()
        {
            return this.changes;
        }

        public void MarkChangesAsCommitted()
        {
            this.currentVersion += this.changes.Count;
            this.changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<DomainEvent> history)
        {
            foreach (var e in history)
            {
                this.ApplyChange(e, false);
                this.currentVersion++;
            }
        }

        protected void ApplyChange(DomainEvent @event)
        {
            this.ApplyChange(@event, true);
        }

        private void ApplyChange(DomainEvent @event, bool isNew)
        {
            // call the private Apply method
            try
            {
                this.GetType().InvokeMember("Apply", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, this, new[] { @event });
            }
            catch (MissingMethodException)
            {
                // do nothing. This just means that an Apply method was not implemented
                // because the state is not needed within the domain model
            }

            if (isNew)
            {
                this.changes.Add(@event);
            }
        }

        private int _id;

        public virtual int Id
        {
            get
            {
                return this._id;
            }

            protected set
            {
                this._id = value;
            }
        }

        public int CurrentVersion
        {
            get { return this.currentVersion; }
        }
    }
}
