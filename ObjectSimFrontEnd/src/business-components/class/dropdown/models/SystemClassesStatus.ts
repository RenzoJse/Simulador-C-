import OptionComponent from '../../../../components/dropdown/models/option.component';
export default interface SystemClassesStatus {
    loading?: true;
    systemClasses: Array<OptionComponent>;
    error?: string;
}
