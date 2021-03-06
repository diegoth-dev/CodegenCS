﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("ProductListPriceHistory", Schema = "Production")]
    public partial class ProductListPriceHistory : INotifyPropertyChanged
    {
        #region Members
        private int _productId;
        [Key]
        public int ProductId 
        { 
            get { return _productId; } 
            set { SetField(ref _productId, value, nameof(ProductId)); } 
        }
        private DateTime _startDate;
        [Key]
        public DateTime StartDate 
        { 
            get { return _startDate; } 
            set { SetField(ref _startDate, value, nameof(StartDate)); } 
        }
        private DateTime? _endDate;
        public DateTime? EndDate 
        { 
            get { return _endDate; } 
            set { SetField(ref _endDate, value, nameof(EndDate)); } 
        }
        private decimal _listPrice;
        public decimal ListPrice 
        { 
            get { return _listPrice; } 
            set { SetField(ref _listPrice, value, nameof(ListPrice)); } 
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate 
        { 
            get { return _modifiedDate; } 
            set { SetField(ref _modifiedDate, value, nameof(ModifiedDate)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (ProductId == default(int) && StartDate == default(DateTime))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Production].[ProductListPriceHistory]
                (
                    [EndDate],
                    [ListPrice],
                    [ModifiedDate],
                    [ProductID],
                    [StartDate]
                )
                VALUES
                (
                    @EndDate,
                    @ListPrice,
                    @ModifiedDate,
                    @ProductId,
                    @StartDate
                )";

                conn.Execute(cmd, this);
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Production].[ProductListPriceHistory] SET
                    [EndDate] = @EndDate,
                    [ListPrice] = @ListPrice,
                    [ModifiedDate] = @ModifiedDate,
                    [ProductID] = @ProductId,
                    [StartDate] = @StartDate
                WHERE
                    [ProductID] = @ProductId AND 
                    [StartDate] = @StartDate";
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
            ProductListPriceHistory other = obj as ProductListPriceHistory;
            if (other == null) return false;

            if (EndDate != other.EndDate)
                return false;
            if (ListPrice != other.ListPrice)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (ProductId != other.ProductId)
                return false;
            if (StartDate != other.StartDate)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (EndDate == null ? 0 : EndDate.GetHashCode());
                hash = hash * 23 + (ListPrice == default(decimal) ? 0 : ListPrice.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (ProductId == default(int) ? 0 : ProductId.GetHashCode());
                hash = hash * 23 + (StartDate == default(DateTime) ? 0 : StartDate.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(ProductListPriceHistory left, ProductListPriceHistory right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProductListPriceHistory left, ProductListPriceHistory right)
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
