import { RouterOutlet } from "./router";
import { AuthorizedRouteMiddleware } from "./users";

export class AppRouterOutletComponent extends RouterOutlet {
    constructor(el: any) {
        super(el);
    }

    connectedCallback() {
        this.setRoutes([
            { path: "/", name: "booking-master-detail", authRequired: true },
            { path: "/tab/:tabIndex", name: "booking-master-detail", authRequired: true },
            { path: "/booking/edit/:bookingId/tab/:tabIndex", name: "booking-master-detail", authRequired: true },
            { path: "/booking/edit/:bookingId", name: "booking-master-detail", authRequired: true },

            { path: "/resources", name: "resource-master-detail", authRequired: true },

            { path: "/register", name: "account-register" },
            { path: "/login", name: "login" },
            { path: "/error", name: "error" },
            { path: "*", name: "not-found" }
        ] as any);

        this.use(new AuthorizedRouteMiddleware());

        super.connectedCallback();
    }

}

customElements.define(`ce-app-router-oulet`, AppRouterOutletComponent);