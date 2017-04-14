export class Resource { 
    public id:any;
    public name:string;

    public fromJSON(data: { name:string }): Resource {
        let resource = new Resource();
        resource.name = data.name;
        return resource;
    }
}
