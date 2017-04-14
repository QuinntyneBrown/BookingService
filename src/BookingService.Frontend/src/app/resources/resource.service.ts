import { fetch } from "../utilities";
import { Resource } from "./resource.model";

export class ResourceService {
    constructor(private _fetch = fetch) { }

    private static _instance: ResourceService;

    public static get Instance() {
        this._instance = this._instance || new ResourceService();
        return this._instance;
    }

    public get(): Promise<Array<Resource>> {
        return this._fetch({ url: "/api/resource/get", authRequired: true }).then((results:string) => {
            return (JSON.parse(results) as { resources: Array<Resource> }).resources;
        });
    }

    public getById(id): Promise<Resource> {
        return this._fetch({ url: `/api/resource/getbyid?id=${id}`, authRequired: true }).then((results:string) => {
            return (JSON.parse(results) as { resource: Resource }).resource;
        });
    }

    public add(resource) {
        return this._fetch({ url: `/api/resource/add`, method: "POST", data: { resource }, authRequired: true  });
    }

    public remove(options: { id : number }) {
        return this._fetch({ url: `/api/resource/remove?id=${options.id}`, method: "DELETE", authRequired: true  });
    }
    
}
