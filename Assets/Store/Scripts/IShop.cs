namespace Store.Scripts
{
    public interface IShop
    {
        /// <summary>
        /// Enables grabbing on all the items
        /// </summary>
        public void EnableGrab();
        
        /// <summary>
        /// Displays the UI panels for the item's
        /// name, price, description, and the
        /// money the player has and the total cost of the items in the
        /// shopping cart.
        /// </summary>
        public void ShowPanels(Item item);
        
        /// <summary>
        /// Add an item to the shopping cart, add its cost to the
        /// checkout value, and send it to shopDialogBehaviour to display
        /// 
        /// Triggered by item entering shopping cart
        /// </summary>
        public void AddToCart(Item item);
        
        /// <summary>
        /// Remove an item from the shopping cart, remove its cost from the
        /// checkout value, and send it to shopDialogBehaviour to display
        ///
        /// Triggered by item leaving shopping cart
        /// </summary>
        public void RemoveFromCart(Item item);

        /// <summary>
        /// Add all the items to the player's inventory and
        /// subtract their total price from the player's money.
        ///
        /// If the player does not have enough money, return false.
        /// Else, return true. (this is used to display error messages)
        /// </summary>
        /// <returns>whether the checkout was successful.</returns>
        public bool Checkout();
        
    }
}
