export class EventLocation { 
    public id:any;
    public name:string;
    public distance: any;

    public fromJSON(data: { name:string }): EventLocation {
        let eventLocation = new EventLocation();
        eventLocation.name = data.name;
        return eventLocation;
    }
}
