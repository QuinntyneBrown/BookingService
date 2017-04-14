import { fetch } from "../utilities";

import { environment } from "../environment";

export class ApiService {
    constructor(private _fetch = fetch) { }

    private static _instance: ApiService;

    public static get Instance() {
        this._instance = this._instance || new this();
        return this._instance;
    }

    public getAddress(options: {
        longitude: number,
        latitude: number
    }): Promise<string> {         
        return this._fetch({ url: `/api/geolocation/getAddress?longitude=${options.longitude}&latitude=${options.latitude}`, authRequired: false }).then((results: string) => {
            return (JSON.parse(results) as { address: string }).address as string;
        });
    }   

    public getClosetEvents(options: { address: string }): Promise<Array<any>> {
        return this._fetch({ url: `/api/event/getClosest?address=${options.address}`, authRequired: false }).then((results: string) => {
            return (JSON.parse(results) as { events: Array<any> }).events as Array<any>;
        });
    }  
}
