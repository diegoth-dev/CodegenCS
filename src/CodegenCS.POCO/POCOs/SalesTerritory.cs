﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("SalesTerritory", Schema = "Sales")]
    public partial class SalesTerritory : INotifyPropertyChanged
    {
        #region Members
        private int _territoryId;
        [Key]
        public int TerritoryId 
        { 
            get { return _territoryId; } 
            set { SetField(ref _territoryId, value, nameof(TerritoryId)); } 
        }
        private decimal _costLastYear;
        public decimal CostLastYear 
        { 
            get { return _costLastYear; } 
            set { SetField(ref _costLastYear, value, nameof(CostLastYear)); } 
        }
        private decimal _costYtd;
        public decimal CostYtd 
        { 
            get { return _costYtd; } 
            set { SetField(ref _costYtd, value, nameof(CostYtd)); } 
        }
        private string _countryRegionCode;
        public string CountryRegionCode 
        { 
            get { return _countryRegionCode; } 
            set { SetField(ref _countryRegionCode, value, nameof(CountryRegionCode)); } 
        }
        private string _group;
        public string Group 
        { 
            get { return _group; } 
            set { SetField(ref _group, value, nameof(Group)); } 
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate 
        { 
            get { return _modifiedDate; } 
            set { SetField(ref _modifiedDate, value, nameof(ModifiedDate)); } 
        }
        private string _name;
        public string Name 
        { 
            get { return _name; } 
            set { SetField(ref _name, value, nameof(Name)); } 
        }
        private Guid _rowguid;
        public Guid Rowguid 
        { 
            get { return _rowguid; } 
            set { SetField(ref _rowguid, value, nameof(Rowguid)); } 
        }
        private decimal _salesLastYear;
        public decimal SalesLastYear 
        { 
            get { return _salesLastYear; } 
            set { SetField(ref _salesLastYear, value, nameof(SalesLastYear)); } 
        }
        private decimal _salesYtd;
        public decimal SalesYtd 
        { 
            get { return _salesYtd; } 
            set { SetField(ref _salesYtd, value, nameof(SalesYtd)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (TerritoryId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Sales].[SalesTerritory]
                (
                    [CostLastYear],
                    [CostYTD],
                    [CountryRegionCode],
                    [Group],
                    [ModifiedDate],
                    [Name],
                    [SalesLastYear],
                    [SalesYTD]
                )
                VALUES
                (
                    @CostLastYear,
                    @CostYtd,
                    @CountryRegionCode,
                    @Group,
                    @ModifiedDate,
                    @Name,
                    @SalesLastYear,
                    @SalesYtd
                )";

                this.TerritoryId = conn.Query<int>(cmd + "SELECT SCOPE_IDENTITY();", this).Single();
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Sales].[SalesTerritory] SET
                    [CostLastYear] = @CostLastYear,
                    [CostYTD] = @CostYtd,
                    [CountryRegionCode] = @CountryRegionCode,
                    [Group] = @Group,
                    [ModifiedDate] = @ModifiedDate,
                    [Name] = @Name,
                    [SalesLastYear] = @SalesLastYear,
                    [SalesYTD] = @SalesYtd
                WHERE
                    [TerritoryID] = @TerritoryId";
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
            SalesTerritory other = obj as SalesTerritory;
            if (other == null) return false;

            if (CostLastYear != other.CostLastYear)
                return false;
            if (CostYtd != other.CostYtd)
                return false;
            if (CountryRegionCode != other.CountryRegionCode)
                return false;
            if (Group != other.Group)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (Name != other.Name)
                return false;
            if (Rowguid != other.Rowguid)
                return false;
            if (SalesLastYear != other.SalesLastYear)
                return false;
            if (SalesYtd != other.SalesYtd)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (CostLastYear == default(decimal) ? 0 : CostLastYear.GetHashCode());
                hash = hash * 23 + (CostYtd == default(decimal) ? 0 : CostYtd.GetHashCode());
                hash = hash * 23 + (CountryRegionCode == null ? 0 : CountryRegionCode.GetHashCode());
                hash = hash * 23 + (Group == null ? 0 : Group.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (Name == null ? 0 : Name.GetHashCode());
                hash = hash * 23 + (Rowguid == default(Guid) ? 0 : Rowguid.GetHashCode());
                hash = hash * 23 + (SalesLastYear == default(decimal) ? 0 : SalesLastYear.GetHashCode());
                hash = hash * 23 + (SalesYtd == default(decimal) ? 0 : SalesYtd.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(SalesTerritory left, SalesTerritory right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SalesTerritory left, SalesTerritory right)
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
