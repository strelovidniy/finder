import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export default class FormValidators {
    public static matchValidator(source: string, target: string): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {

            const sourceCtrl = control.get(source) || control.root.get(source);
            const targetCtrl = control.get(target) || control.root.get(target);

            return sourceCtrl && targetCtrl && sourceCtrl.value !== targetCtrl.value
                ? { mismatch: true }
                : null;
        };
    }

    public static patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } => {
            if (!control.value) {
            // if control is empty return no error
                return null as any as { [key: string]: any };
            }

            // test the value of the control against the regexp supplied
            const valid = regex.test(control.value);

            // if true, return no error (no error), else return error passed in the second parameter
            return valid ? null as any as { [key: string]: any } : error;
        };
    }

    public static validateProperty(property: string): ValidatorFn {
        return (control: AbstractControl): {[key: string]: any} | null => {
            const propertyVal = control.value && control.value[property];

            return !propertyVal? { 'invalidProperty': true } : null;
        };

    }

    public static cannotContainSpace(control: AbstractControl): ValidationErrors | null {
        if((control.value as string).indexOf(' ') >= 0){
            return { cannotContainSpace: true };
        }

        return null;
    }
}
