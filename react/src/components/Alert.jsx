import * as React from 'react';
import Alert from 'react-bootstrap/Alert';
import Button from 'react-bootstrap/Button';

export default function RedAlert() {
    
    return (
        <Alert dismissible variant="danger">
            <Alert.Heading>Fuck!</Alert.Heading>
            <p>Something happened</p>
            <hr/>
            <div className="d-flex justify-content-end">
                <Button variant="outline-success" className="close" data-dismiss="alert" aria-label="Close">
                    Dismiss
                </Button>
            </div>
        </Alert>
    );
}