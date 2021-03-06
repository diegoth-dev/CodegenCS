﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("Address", Schema = "Person")]
    public partial class Address : INotifyPropertyChanged
    {
        #region Members
        private int _addressId;
        [Key]
        public int AddressId 
        { 
            get { return _addressId; } 
            set { SetField(ref _addressId, value, nameof(AddressId)); } 
        }
        private string _addressLine1;
        public string AddressLine1 
        { 
            get { return _addressLine1; } 
            set { SetField(ref _addressLine1, value, nameof(AddressLine1)); } 
        }
        private string _addressLine2;
        public string AddressLine2 
        { 
            get { return _addressLine2; } 
            set { SetField(ref _addressLine2, value, nameof(AddressLine2)); } 
        }
        private string _city;
        public string City 
        { 
            get { return _city; } 
            set { SetField(ref _city, value, nameof(City)); } 
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate 
        { 
            get { return _modifiedDate; } 
            set { SetField(ref _modifiedDate, value, nameof(ModifiedDate)); } 
        }
        private string _postalCode;
        public string PostalCode 
        { 
            get { return _postalCode; } 
            set { SetField(ref _postalCode, value, nameof(PostalCode)); } 
        }
        private Guid _rowguid;
        public Guid Rowguid 
        { 
            get { return _rowguid; } 
            set { SetField(ref _rowguid, value, nameof(Rowguid)); } 
        }
        private int _stateProvinceId;
        public int StateProvinceId 
        { 
            get { return _stateProvinceId; } 
            set { SetField(ref _stateProvinceId, value, nameof(StateProvinceId)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (AddressId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Person].[Address]
                (
                    [AddressLine1],
                    [AddressLine2],
                    [City],
                    [ModifiedDate],
                    [PostalCode],
                    [StateProvinceID]
                )
                VALUES
                (
                    @AddressLine1,
                    @AddressLine2,
                    @City,
                    @ModifiedDate,
                    @PostalCode,
                    @StateProvinceId
                )";

                this.AddressId = conn.Query<int>(cmd + "SELECT SCOPE_IDENTITY();", this).Single();
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Person].[Address] SET
                    [AddressLine1] = @AddressLine1,
                    [AddressLine2] = @AddressLine2,
                    [City] = @City,
                    [ModifiedDate] = @ModifiedDate,
                    [PostalCode] = @PostalCode,
                    [StateProvinceID] = @StateProvinceId
                WHERE
                    [AddressID] = @AddressId";
                conn.Execute(cmd, this);
            }
        }
        #endregion ActiveRecord

        #region Equals/GetHashCode
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            Address other = obj as Address;
            if (other == null) return false;

            if (AddressLine1 != other.AddressLine1)
                return false;
            if (AddressLine2 != other.AddressLine2)
                return false;
            if (City != other.City)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (PostalCode != other.PostalCode)
                return false;
            if (Rowguid != other.Rowguid)
                return false;
            if (StateProvinceId != other.StateProvinceId)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (AddressLine1 == null ? 0 : AddressLine1.GetHashCode());
                hash = hash * 23 + (AddressLine2 == null ? 0 : AddressLine2.GetHashCode());
                hash = hash * 23 + (City == null ? 0 : City.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (PostalCode == null ? 0 : PostalCode.GetHashCode());
                hash = hash * 23 + (Rowguid == default(Guid) ? 0 : Rowguid.GetHashCode());
                hash = hash * 23 + (StateProvinceId == default(int) ? 0 : StateProvinceId.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(Address left, Address right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !Equals(left, right);
        }

        #endregion Equals/GetHashCode

        #region INotifyPropertyChanged/IsDirty
        public HashSet<string> ChangedProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public void MarkAsClean()
        {
            ChangedProperties.Clear();
        }
        public virtual bool IsDirty => ChangedProperties.Any();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetField<T>(ref T field, T value, string propertyName) {
            if (!EqualityComparer<T>.Default.Equals(field, value)) {
                field = value;
                ChangedProperties.Add(propertyName);
                OnPropertyChanged(propertyName);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged/IsDirty
    }
}
