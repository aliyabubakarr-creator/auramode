// AURAMODE Global Store & State Management
// Integrated with ASP.NET Core REST API & SQLite Backend

let PRODUCTS = [
  {
    id: 'mushroom-cosmic-white',
    name: 'Cosmic Tee',
    subtitle: 'T-Shirt',
    price: 15000,
    category: 'wears',
    collection: 'SZN 2',
    badge: 'NEW',
    image: 'assets/product-cosmic-white.png',
    gallery: [
      'assets/product-detail-front.png',
      'assets/product-detail-models.png',
      'assets/product-detail-closeup.png',
      'assets/product-detail-back.png'
    ],
    colors: ['White', 'Black'],
    sizes: ['S', 'M', 'L', 'XL', '2XL'],
    inStock: true,
    stockQuantity: 45,
    description: 'Signature graphic tee crafted from 280GSM heavyweight combed organic cotton. Features screenprinted cosmic mushroom motifs and custom relaxed boxy fit.'
  },
  {
    id: 'mushroom-duo-models',
    name: 'Campaign Tee',
    subtitle: 'T-Shirt',
    price: 15000,
    category: 'wears',
    collection: 'SZN 1',
    badge: 'NEW',
    image: 'assets/product-models-white.png',
    gallery: [
      'assets/product-detail-models.png',
      'assets/product-detail-front.png',
      'assets/product-detail-closeup.png',
      'assets/product-detail-back.png'
    ],
    colors: ['White', 'Black'],
    sizes: ['S', 'M', 'L', 'XL'],
    inStock: true,
    stockQuantity: 30,
    description: 'Campaign edition graphic tee featuring double front placement and ribbed crewneck.'
  },
  {
    id: 'auramode-trucker-hat',
    name: 'Trucker Cap',
    subtitle: 'Headwear',
    price: 15000,
    category: 'accessories',
    collection: 'SZN 3',
    badge: '',
    image: 'assets/product-cap.png',
    gallery: ['assets/product-cap.png'],
    colors: ['Grey/White', 'Black'],
    sizes: ['One Size'],
    inStock: true,
    stockQuantity: 50,
    description: 'Branded trucker hat with mesh back, curved brim and embroidered AURAMODE front script.'
  },
  {
    id: 'mushroom-seated-black',
    name: 'Vintage Black Tee',
    subtitle: 'T-Shirt',
    price: 15000,
    category: 'wears',
    collection: 'SZN 1',
    badge: 'NEW',
    image: 'assets/product-seated-black.png',
    gallery: [
      'assets/product-seated-black.png',
      'assets/product-boxlogo-black.png'
    ],
    colors: ['Black', 'White'],
    sizes: ['S', 'M', 'L', 'XL'],
    inStock: true,
    stockQuantity: 25,
    description: 'Washed vintage black heavy jersey with photographic center back print and subtle breast pocket print.'
  },
  {
    id: 'auramode-boxlogo-black',
    name: 'Box Logo Tee',
    subtitle: 'T-Shirt',
    price: 15000,
    category: 'wears',
    collection: 'SZN 2',
    badge: 'NEW',
    image: 'assets/product-boxlogo-black.png',
    gallery: ['assets/product-boxlogo-black.png'],
    colors: ['Black', 'White'],
    sizes: ['S', 'M', 'L', 'XL'],
    inStock: true,
    stockQuantity: 35,
    description: 'Minimalist brutalist box emblem screenprint on deep black heavyweight cotton.'
  },
  {
    id: 'mushroom-nature-white',
    name: 'Botanical Tee',
    subtitle: 'T-Shirt',
    price: 15000,
    category: 'wears',
    collection: 'SZN 1',
    badge: 'SOLD OUT',
    image: 'assets/product-nature-white.png',
    gallery: ['assets/product-nature-white.png'],
    colors: ['White'],
    sizes: ['S', 'M', 'L'],
    inStock: false,
    stockQuantity: 0,
    description: 'Botanical archive print white tee. Limited archival release.'
  },
  {
    id: 'target-silhouette-white',
    name: 'Target Silhouette',
    subtitle: 'T-Shirt',
    price: 15000,
    category: 'wears',
    collection: 'SZN 3',
    badge: 'SOLD OUT',
    image: 'assets/product-target-white.png',
    gallery: ['assets/product-target-white.png'],
    colors: ['White'],
    sizes: ['S', 'M', 'L'],
    inStock: false,
    stockQuantity: 0,
    description: 'Target crosshair silhouette graphic on optic white tee.'
  },
  {
    id: 'auramode-grey-heavyweight',
    name: 'Heavyweight Grey',
    subtitle: 'T-Shirt',
    price: 15000,
    category: 'wears',
    collection: 'SZN 4',
    badge: '',
    image: 'assets/product-grey-tee.png',
    gallery: ['assets/product-grey-tee.png'],
    colors: ['Grey'],
    sizes: ['S', 'M', 'L', 'XL'],
    inStock: true,
    stockQuantity: 60,
    description: 'Upcoming SZN 4 staple grey tee with oval AURAMODE chest insignia.'
  }
];

class Store {
  constructor() {
    this.initCart();
    this.initWishlist();
    this.initAuth();
    this.initOrders();
    this.promoCode = localStorage.getItem('auramode_promo') || '';
    this.syncProductsFromApi();
    this.syncOrdersFromApi();
  }

  // Sync Products from Backend API
  async syncProductsFromApi() {
    try {
      const res = await fetch('/api/products');
      if (res.ok) {
        const data = await res.json();
        if (Array.isArray(data) && data.length > 0) {
          PRODUCTS.splice(0, PRODUCTS.length, ...data);
          this.dispatchUpdate();
        }
      }
    } catch (e) {
      // Offline fallback: keep static products
    }
  }

  // --- Cart ---
  initCart() {
    const saved = localStorage.getItem('auramode_cart');
    if (saved) {
      this.cart = JSON.parse(saved);
    } else {
      this.cart = [
        { id: 'mushroom-cosmic-white', name: 'Cosmic Tee', color: 'White', size: 'M', price: 15000, qty: 1, image: 'assets/product-cosmic-white.png' },
        { id: 'mushroom-seated-black', name: 'Vintage Black Tee', color: 'Black', size: 'M', price: 15000, qty: 1, image: 'assets/product-seated-black.png' },
        { id: 'auramode-boxlogo-black', name: 'Box Logo Tee', color: 'Black', size: 'M', price: 15000, qty: 1, image: 'assets/product-boxlogo-black.png' },
        { id: 'auramode-trucker-hat', name: 'Trucker Cap', color: 'Grey/White', size: 'One Size', price: 15000, qty: 1, image: 'assets/product-cap.png' }
      ];
      this.saveCart();
    }
  }

  saveCart() {
    localStorage.setItem('auramode_cart', JSON.stringify(this.cart));
    this.dispatchUpdate();
  }

  addToCart(product, color = 'White', size = 'M', qty = 1) {
    const existingIndex = this.cart.findIndex(item => item.id === product.id && item.color === color && item.size === size);
    if (existingIndex > -1) {
      this.cart[existingIndex].qty += qty;
    } else {
      this.cart.push({
        id: product.id,
        name: `${product.name} ${product.subtitle}`,
        color: color,
        size: size,
        price: product.price,
        qty: qty,
        image: product.image
      });
    }
    this.saveCart();
  }

  removeFromCart(index) {
    this.cart.splice(index, 1);
    this.saveCart();
  }

  updateQty(index, delta) {
    if (this.cart[index]) {
      this.cart[index].qty += delta;
      if (this.cart[index].qty <= 0) {
        this.removeFromCart(index);
      } else {
        this.saveCart();
      }
    }
  }

  getCartCount() {
    return this.cart.reduce((sum, item) => sum + item.qty, 0);
  }

  getSubtotal() {
    return this.cart.reduce((sum, item) => sum + (item.price * item.qty), 0);
  }

  getShipping() {
    const subtotal = this.getSubtotal();
    return (subtotal >= 15000 || subtotal === 0) ? 0 : 5000;
  }

  getTotal() {
    let total = this.getSubtotal() + this.getShipping();
    if (this.promoCode === 'AURA10') {
      total = Math.round(total * 0.9);
    }
    return total;
  }

  applyPromo(code) {
    if (code.toUpperCase() === 'AURA10') {
      this.promoCode = 'AURA10';
      localStorage.setItem('auramode_promo', 'AURA10');
      return { success: true, message: '10% discount applied!' };
    }
    return { success: false, message: 'Invalid promo code' };
  }

  // --- Wishlist ---
  initWishlist() {
    const saved = localStorage.getItem('auramode_wishlist');
    if (saved) {
      this.wishlist = JSON.parse(saved);
    } else {
      this.wishlist = ['mushroom-cosmic-white', 'mushroom-duo-models', 'auramode-trucker-hat', 'mushroom-seated-black', 'auramode-boxlogo-black', 'mushroom-nature-white'];
      this.saveWishlist();
    }
  }

  saveWishlist() {
    localStorage.setItem('auramode_wishlist', JSON.stringify(this.wishlist));
    this.dispatchUpdate();
  }

  toggleWishlist(productId) {
    const idx = this.wishlist.indexOf(productId);
    if (idx > -1) {
      this.wishlist.splice(idx, 1);
    } else {
      this.wishlist.push(productId);
    }
    this.saveWishlist();
  }

  isInWishlist(productId) {
    return this.wishlist.includes(productId);
  }

  getWishlistCount() {
    return this.wishlist.length;
  }

  // --- Auth ---
  initAuth() {
    const saved = localStorage.getItem('auramode_user');
    if (saved) {
      this.user = JSON.parse(saved);
    } else {
      this.user = {
        name: 'Sherlock P',
        email: 'auramodeszn@gmail.com',
        phone: '09024462630',
        address: 'River Park Estate, Lugbe',
        city: 'Abuja',
        state: 'FCT',
        loggedIn: true,
        id: 'user-sherlock-default'
      };
      localStorage.setItem('auramode_user', JSON.stringify(this.user));
    }
  }

  async login(email, password) {
    try {
      const res = await fetch('/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
      });

      if (res.ok) {
        const data = await res.json();
        this.user = data.user;
        localStorage.setItem('auramode_user', JSON.stringify(this.user));
        this.dispatchUpdate();
        await this.syncOrdersFromApi();
        return { success: true, user: this.user };
      }
      return { success: false, message: 'Invalid email or password' };
    } catch (err) {
      // Offline fallback
      this.user = { name: email.split('@')[0], email: email, loggedIn: true };
      localStorage.setItem('auramode_user', JSON.stringify(this.user));
      this.dispatchUpdate();
      return { success: true, user: this.user };
    }
  }

  async register(formData) {
    try {
      const res = await fetch('/api/auth/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });

      if (res.ok) {
        const data = await res.json();
        this.user = data.user;
        localStorage.setItem('auramode_user', JSON.stringify(this.user));
        this.dispatchUpdate();
        return { success: true, user: this.user };
      }

      const err = await res.json();
      return { success: false, message: err.message || 'Registration failed' };
    } catch (err) {
      // Offline fallback
      this.user = { name: `${formData.firstName} ${formData.lastName}`, email: formData.email, loggedIn: true };
      localStorage.setItem('auramode_user', JSON.stringify(this.user));
      this.dispatchUpdate();
      return { success: true, user: this.user };
    }
  }

  async updateProfile(profileData) {
    try {
      const res = await fetch('/api/auth/profile', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(profileData)
      });
      if (res.ok) {
        const data = await res.json();
        this.user = data.user;
        localStorage.setItem('auramode_user', JSON.stringify(this.user));
        this.dispatchUpdate();
        return { success: true, user: this.user };
      }
    } catch (e) {
      // Offline fallback
    }

    this.user = { ...this.user, ...profileData };
    localStorage.setItem('auramode_user', JSON.stringify(this.user));
    this.dispatchUpdate();
    return { success: true, user: this.user };
  }

  logout() {
    this.user = { name: 'Guest', email: '', loggedIn: false };
    localStorage.setItem('auramode_user', JSON.stringify(this.user));
    this.dispatchUpdate();
  }

  // --- Orders ---
  initOrders() {
    const saved = localStorage.getItem('auramode_orders');
    if (saved) {
      this.orders = JSON.parse(saved);
    } else {
      this.orders = [
        {
          orderNo: '22445588',
          status: 'SENT',
          date: '22/11/2023',
          total: 60000,
          items: [
            { name: 'Cosmic Tee', image: 'assets/product-cosmic-white.png', qty: 1, price: 15000 },
            { name: 'Vintage Black Tee', image: 'assets/product-seated-black.png', qty: 1, price: 15000 },
            { name: 'Box Logo Tee', image: 'assets/product-boxlogo-black.png', qty: 1, price: 15000 },
            { name: 'Trucker Cap', image: 'assets/product-cap.png', qty: 1, price: 15000 }
          ]
        },
        {
          orderNo: '22445589',
          status: 'SENT',
          date: '22/11/2023',
          total: 30000,
          items: [
            { name: 'Cosmic Tee', image: 'assets/product-cosmic-white.png', qty: 1, price: 15000 },
            { name: 'Vintage Black Tee', image: 'assets/product-seated-black.png', qty: 1, price: 15000 }
          ]
        },
        {
          orderNo: '22445590',
          status: 'SENT',
          date: '22/11/2023',
          total: 60000,
          items: [
            { name: 'Cosmic Tee', image: 'assets/product-cosmic-white.png', qty: 1, price: 15000 },
            { name: 'Vintage Black Tee', image: 'assets/product-seated-black.png', qty: 1, price: 15000 },
            { name: 'Box Logo Tee', image: 'assets/product-boxlogo-black.png', qty: 1, price: 15000 },
            { name: 'Trucker Cap', image: 'assets/product-cap.png', qty: 1, price: 15000 }
          ]
        }
      ];
      localStorage.setItem('auramode_orders', JSON.stringify(this.orders));
    }
  }

  async syncOrdersFromApi() {
    try {
      const email = this.user && this.user.email ? this.user.email : '';
      const url = email ? `/api/orders?email=${encodeURIComponent(email)}` : '/api/orders';
      const res = await fetch(url);
      if (res.ok) {
        const data = await res.json();
        if (Array.isArray(data) && data.length > 0) {
          this.orders = data;
          localStorage.setItem('auramode_orders', JSON.stringify(this.orders));
          this.dispatchUpdate();
        }
      }
    } catch (e) {
      // Offline fallback
    }
  }

  async createOrder(shippingDetails, paymentReference = '') {
    const payload = {
      userId: this.user && this.user.id ? this.user.id : null,
      email: shippingDetails.email || (this.user ? this.user.email : ''),
      firstName: shippingDetails.firstName || (shippingDetails.name ? shippingDetails.name.split(' ')[0] : ''),
      lastName: shippingDetails.lastName || (shippingDetails.name ? shippingDetails.name.split(' ').slice(1).join(' ') : ''),
      phone: shippingDetails.phone || '',
      address: shippingDetails.address || '',
      city: shippingDetails.city || '',
      state: shippingDetails.state || '',
      deliveryMethod: shippingDetails.delivery || 'standard',
      paymentMethod: shippingDetails.paymentMethod || 'paystack',
      paymentReference: paymentReference,
      total: this.getTotal(),
      items: [...this.cart]
    };

    let newOrder = null;
    try {
      const res = await fetch('/api/orders', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });

      if (res.ok) {
        const data = await res.json();
        newOrder = {
          orderNo: data.orderNo,
          status: data.status,
          date: data.date,
          total: data.total,
          items: [...this.cart],
          shipping: shippingDetails,
          paymentReference: paymentReference
        };
      }
    } catch (e) {
      // Offline fallback
    }

    if (!newOrder) {
      newOrder = {
        orderNo: Math.floor(10000000 + Math.random() * 90000000).toString(),
        status: paymentReference ? 'PAID' : 'PROCESSING',
        date: new Date().toLocaleDateString('en-GB'),
        total: this.getTotal(),
        items: [...this.cart],
        shipping: shippingDetails,
        paymentReference: paymentReference
      };
    }

    this.orders.unshift(newOrder);
    localStorage.setItem('auramode_orders', JSON.stringify(this.orders));
    this.cart = [];
    this.saveCart();
    return newOrder;
  }

  async initializePaystackPayment(email, total) {
    try {
      const res = await fetch('/api/payments/paystack/initialize', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, total, items: this.cart })
      });
      if (res.ok) {
        return await res.json();
      }
    } catch (e) {
      // Fallback
    }

    return {
      success: true,
      reference: `AURA_${Date.now()}_${Math.floor(Math.random() * 9000 + 1000)}`,
      publicKey: 'pk_test_auramode_demo_public_key'
    };
  }

  async verifyPayment(reference, orderNo = '') {
    try {
      const res = await fetch('/api/payments/paystack/verify', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ reference, orderNo })
      });
      if (res.ok) {
        return await res.json();
      }
    } catch (e) {
      // Offline fallback
    }
    return { success: true, reference };
  }

  dispatchUpdate() {
    window.dispatchEvent(new CustomEvent('auramode_store_updated'));
  }
}

const store = new Store();
