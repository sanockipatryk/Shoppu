
# Shoppu application
Online clothing store application.

## Application

Application built using .NET 6 with code-first approach and the use of Razor Pages with CQRS and Mediator Pattern. Client side of the application was built using Razor syntax with Bulma, Jquery and Javascript. Database using Microsoft SQL Server, database querying using Entity Framework. Application authorization utilizes .NET Identity.
Application uses the MediatR Nuget package.

Application is currently hosted using Azure. To access it, click the link below: 

(Server might need a few seconds to get back up)

[Hosted application](https://shoppu.azurewebsites.net/)

Users are welcome to create their own accounts, or use one of already existing ones:

**admin**: 

login: admin@admin.com 

password: !Q1w2e3r4

**user**: 

login: user@user.com

password: !Q1w2e3r4

**user2**

login: user2@user.com

password: !Q1w2e3r4

Application was fully equipped to fully display the application ecosystem.

### Description

Application was designed to mimic a clothing store with various different possibilities.

Website was built for desktop as well as mobile users, with some exceptions for admin product management, where the UI needs a desktop type of screen to be fully utilised.

#### Admins

Admins of the application have control over the site content - they can create new color variants, manage categories, create products and their different color variants, as well as manage their sizes and stock.

#### Users

Users can create their accounts, browse website, using different product categories and filters provided, add items to cart, place orders as well as check out their previous orders.

#### Categories

Categories are hierarchical - every category can have a parent category, and subcategories of its own, which makes it easier for user to find items he might be looking for.

Product categories are separated by gender. Category can be shared between the two genders, allowing for adding products that target different genders, to one category.
Categories can also have gender specified - making them be displayed only to the selected gender, and forcing adding products that target only the specified gender.

If category has its own subcategories, it cannot contain products of its own, but the subcategories can, and the parent category acts just as a grouping device for different products.

#### Products

Products are separated by categories they belong to. Users can browse these categories with the use of dynamically generated navigation menus.

#### Product sizes

When creating product, admin chooses a sizing system for it - By letters (XS, S, M...), numbers, range-like(36-39, 40-42...), or one size, for items that come in only one size.

#### Product variants

Every product can have more than one variant of it, making it easier for user to find a product he likes, but in different color.
When creating a variant, admin adds photos of the item to display on the page.

Products have their name and price specified, but a variant can override it, so that different variants of the same product may have different names, or prices.

Every variant has its own set of possible sizes, and different amount of items in stock.

Product variants are displayed to user after specifying their sizes and making it "accessible" by admin.
Only accessible items can be accessed and added to cart and ordered.

#### Color variants
Every color has its name and color HEX code. Admins can add new possible colors. The colors are displayed in product site to user to make it easier to browse through possible variants.

#### Browsing products
Users can look for products under categories and their subcategories, as well as use filters (by cost, color, size...), sort products and change the amount of items displayed per page.

#### Cart

Users can add items to cart in order to later place an order. Cart can contain multiple product variants in multiple sizes. While viewing cart, user can add more of already added items, remove items that he does not want to buy anymore, or clear the whole cart.

When visiting the cart page, or calling any action on products in cart, the cart will be automatically updated, based on changes to the stock - for example if a size of particular item runs out before the user has a chance of making an order, the user will be notified, and his cart updated. The user will also be notified if he tried to add a number of one item, that exceeds the stock number, making it easy for the user to know whether the product is running out or not.

#### Order

After adding items to the cart, the user can proceed to shipment and payment, where after inputting the address data, and selecting the payment method, the order is created and is considered done and paid.

If any of items in cart run out while ordering, the user, the order will not go through - the cart will get updated, user will get notified and asked to check the content of his cart before continuing with the order.

#### Order details

User has access to his past orders, where he can find details about bought products, their quantity and cost, and the address and payment information that he provided.


Just for the sake of setting up the test website, products and their variants, as well as the source of images was inspired by online shop Reserved.
