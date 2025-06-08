
@Injectable({
    providedIn: 'root',
})
export class ClassApiRepository extends ApiRepository {

    constructor(http: HttpClient){
        super(a.objectsim, 'api/class', http);
    }
    s
    getClassById(classId: string): Observable<Class> {
        return this.get<Class>(classId);
        