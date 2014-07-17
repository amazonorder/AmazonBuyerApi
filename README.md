AmazonBuyerApi
==============

A C# API Allowing you to get an amazon buyer account info using only his/her Id &amp; Password.
This is a "completely incomplete API" that can currently only log you in, and return the number of orders. However anyone can add as many function as they wish and I am going to update this very frequently. The usage is very simple:
use AmazonCls.login(...) to login with a Username and Password as well as a optionally a Proxy (set to null if you don't want to use one). Once logged in you can manually get the page's HTML by cli.MPage or get the number of orders by calling AmazonCls.GetOrderNo(...) supplying the logged in client and orderFilter as arguments. Good luck and thanks for reading!
